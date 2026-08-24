CREATE TABLE IF NOT EXISTS encounters (
  id serial PRIMARY KEY,
  patient_id int NOT NULL,
  encounter_date timestamptz default now(),
  notes text,
  created_at timestamptz default now()
);

CREATE TABLE IF NOT EXISTS observations (
  id serial PRIMARY KEY,
  encounter_id int NOT NULL,
  code text,
  value text,
  units text,
  created_at timestamptz default now()
);
