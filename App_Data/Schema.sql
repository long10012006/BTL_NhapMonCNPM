CREATE TABLE IF NOT EXISTS ACCOUNT (
 account_id INTEGER PRIMARY KEY,
 phone_normalized TEXT NOT NULL UNIQUE CHECK(length(phone_normalized)=10 AND substr(phone_normalized,1,1)='0' AND phone_normalized NOT GLOB '*[^0-9]*'),
 display_name TEXT NOT NULL, email TEXT,
 password_hash TEXT NOT NULL,
 role TEXT NOT NULL CHECK(role IN ('CUSTOMER','MANAGER')),
 is_active INTEGER NOT NULL DEFAULT 1 CHECK(is_active IN (0,1)),
 created_at_utc TEXT NOT NULL, version INTEGER NOT NULL DEFAULT 1
);
CREATE TABLE IF NOT EXISTS COURT (
 court_id INTEGER PRIMARY KEY, court_code TEXT NOT NULL UNIQUE,
 court_name TEXT NOT NULL, description TEXT,
 is_hidden INTEGER NOT NULL DEFAULT 0 CHECK(is_hidden IN (0,1)),
 version INTEGER NOT NULL DEFAULT 1
);
CREATE TABLE IF NOT EXISTS TIME_SLOT (
 slot_id INTEGER PRIMARY KEY, slot_code TEXT NOT NULL UNIQUE,
 start_time TEXT NOT NULL, end_time TEXT NOT NULL CHECK(end_time>start_time),
 is_active INTEGER NOT NULL DEFAULT 1 CHECK(is_active IN (0,1))
);
CREATE TABLE IF NOT EXISTS COURT_SCHEDULE (
 schedule_id INTEGER PRIMARY KEY, court_id INTEGER NOT NULL REFERENCES COURT(court_id),
 slot_id INTEGER NOT NULL REFERENCES TIME_SLOT(slot_id), play_date TEXT NOT NULL,
 is_open INTEGER NOT NULL DEFAULT 1 CHECK(is_open IN (0,1)), lock_reason TEXT,
 created_at_utc TEXT NOT NULL, version INTEGER NOT NULL DEFAULT 1,
 UNIQUE(court_id,slot_id,play_date)
);
CREATE TABLE IF NOT EXISTS COURT_PRICE (
 price_id INTEGER PRIMARY KEY, court_id INTEGER NOT NULL REFERENCES COURT(court_id),
 slot_id INTEGER NOT NULL REFERENCES TIME_SLOT(slot_id), valid_from TEXT NOT NULL,
 valid_to TEXT CHECK(valid_to IS NULL OR valid_to>=valid_from),
 hourly_rate INTEGER NOT NULL CHECK(hourly_rate>0 AND typeof(hourly_rate)='integer'),
 created_by_account_id INTEGER NOT NULL REFERENCES ACCOUNT(account_id), created_at_utc TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS BOOKING (
 booking_id INTEGER PRIMARY KEY, booking_code TEXT NOT NULL UNIQUE,
 schedule_id INTEGER NOT NULL REFERENCES COURT_SCHEDULE(schedule_id),
 customer_account_id INTEGER REFERENCES ACCOUNT(account_id),
 created_by_account_id INTEGER NOT NULL REFERENCES ACCOUNT(account_id),
 price_id INTEGER NOT NULL REFERENCES COURT_PRICE(price_id),
 contact_name TEXT NOT NULL, contact_phone TEXT NOT NULL,
 source TEXT NOT NULL CHECK(source IN ('WEB','PHONE','COUNTER')),
 status TEXT NOT NULL CHECK(status IN ('CONFIRMED','CANCELLED','COMPLETED','NO_SHOW')),
 hourly_rate_snapshot INTEGER NOT NULL CHECK(hourly_rate_snapshot>0),
 duration_minutes INTEGER NOT NULL CHECK(duration_minutes=90),
 total_amount INTEGER NOT NULL CHECK(total_amount=(hourly_rate_snapshot*duration_minutes+30)/60),
 request_key TEXT NOT NULL, request_payload_hash BLOB NOT NULL CHECK(length(request_payload_hash)=32),
 created_at_utc TEXT NOT NULL, updated_at_utc TEXT NOT NULL, version INTEGER NOT NULL DEFAULT 1,
 UNIQUE(created_by_account_id,request_key)
);
CREATE UNIQUE INDEX IF NOT EXISTS UX_BOOKING_CONFIRMED ON BOOKING(schedule_id) WHERE status='CONFIRMED';
CREATE INDEX IF NOT EXISTS IX_SCHEDULE_DATE ON COURT_SCHEDULE(play_date,court_id);
CREATE INDEX IF NOT EXISTS IX_PRICE_RANGE ON COURT_PRICE(court_id,slot_id,valid_from,valid_to);
CREATE INDEX IF NOT EXISTS IX_BOOKING_CUSTOMER ON BOOKING(customer_account_id,created_at_utc);
CREATE TABLE IF NOT EXISTS BOOKING_HISTORY (
 history_id INTEGER PRIMARY KEY, booking_id INTEGER NOT NULL REFERENCES BOOKING(booking_id),
 changed_by_account_id INTEGER NOT NULL REFERENCES ACCOUNT(account_id),
 action TEXT NOT NULL CHECK(action IN ('CREATE','RESCHEDULE','CANCEL','COMPLETE','NO_SHOW')),
 old_schedule_id INTEGER REFERENCES COURT_SCHEDULE(schedule_id),
 new_schedule_id INTEGER NOT NULL REFERENCES COURT_SCHEDULE(schedule_id),
 old_status TEXT, new_status TEXT NOT NULL, old_amount INTEGER, new_amount INTEGER NOT NULL,
 reason TEXT, occurred_at_utc TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS AUDIT_LOG (
 audit_id INTEGER PRIMARY KEY, actor_account_id INTEGER REFERENCES ACCOUNT(account_id),
 action TEXT NOT NULL, target_type TEXT NOT NULL, target_id TEXT,
 old_values_json TEXT, new_values_json TEXT, correlation_id TEXT NOT NULL, occurred_at_utc TEXT NOT NULL
);
CREATE TRIGGER IF NOT EXISTS PRICE_NO_OVERLAP_INSERT BEFORE INSERT ON COURT_PRICE
WHEN EXISTS(SELECT 1 FROM COURT_PRICE p WHERE p.court_id=NEW.court_id AND p.slot_id=NEW.slot_id
 AND p.valid_from<=COALESCE(NEW.valid_to,'9999-12-31') AND COALESCE(p.valid_to,'9999-12-31')>=NEW.valid_from)
BEGIN SELECT RAISE(ABORT,'PRICE_OVERLAP'); END;
CREATE TRIGGER IF NOT EXISTS PRICE_NO_OVERLAP_UPDATE BEFORE UPDATE ON COURT_PRICE
WHEN EXISTS(SELECT 1 FROM COURT_PRICE p WHERE p.price_id<>NEW.price_id AND p.court_id=NEW.court_id AND p.slot_id=NEW.slot_id
 AND p.valid_from<=COALESCE(NEW.valid_to,'9999-12-31') AND COALESCE(p.valid_to,'9999-12-31')>=NEW.valid_from)
BEGIN SELECT RAISE(ABORT,'PRICE_OVERLAP'); END;
