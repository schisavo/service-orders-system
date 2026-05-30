-- =========================
-- USERS (AUTH JWT)
-- =========================
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password_hash TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Usuario precargado (IMPORTANTE PARA LA PRUEBA)
INSERT INTO users (username, password_hash)
VALUES ('admin', '$2a$10$fakehashedpasswordexample');

-- =========================
-- TECHNICIANS
-- =========================
CREATE TABLE technicians (
    id SERIAL PRIMARY KEY,
    full_name VARCHAR(100) NOT NULL,
    phone VARCHAR(20) NOT NULL,
    specialty VARCHAR(100) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- =========================
-- CLIENTS
-- =========================
CREATE TABLE clients (
    id SERIAL PRIMARY KEY,
    full_name VARCHAR(100) NOT NULL,
    document_number VARCHAR(50) UNIQUE NOT NULL,
    address VARCHAR(150),
    phone VARCHAR(20) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- =========================
-- SERVICE ORDERS
-- =========================
CREATE TABLE service_orders (
    id SERIAL PRIMARY KEY,
    status VARCHAR(20) NOT NULL CHECK (status IN ('Pendiente', 'En progreso', 'Finalizada')),
    description TEXT NOT NULL,

    technician_id INT NOT NULL,
    client_id INT NOT NULL,

    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_technician
        FOREIGN KEY (technician_id)
        REFERENCES technicians(id)
        ON DELETE RESTRICT,

    CONSTRAINT fk_client
        FOREIGN KEY (client_id)
        REFERENCES clients(id)
        ON DELETE RESTRICT
);

-- =========================
-- JOIN
-- =========================

SELECT 
    so.id,
    so.status,
    so.description,
    so.created_at,

    t.full_name AS technician_name,
    t.specialty,

    c.full_name AS client_name,
    c.document_number

FROM service_orders so
INNER JOIN technicians t ON so.technician_id = t.id
INNER JOIN clients c ON so.client_id = c.id
ORDER BY so.created_at DESC;