CREATE TABLE IF NOT EXISTS invoices (
  id serial PRIMARY KEY,
  patient_id int NOT NULL,
  total_amount numeric(12,2) default 0,
  status text default 'draft',
  created_at timestamptz default now()
);

CREATE TABLE IF NOT EXISTS invoice_lines (
  id serial PRIMARY KEY,
  invoice_id int NOT NULL,
  description text,
  amount numeric(12,2),
  created_at timestamptz default now()
);
