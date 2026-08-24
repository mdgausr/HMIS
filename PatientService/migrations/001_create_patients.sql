CREATE TABLE IF NOT EXISTS patients (
  id serial PRIMARY KEY,
  first_name text NOT NULL,
  last_name text NOT NULL,
  gender text,
  dob date,
  created_at timestamptz default now()
);
