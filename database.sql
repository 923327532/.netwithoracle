-- Base de datos: Taller mecánico (Neon / PostgreSQL)
-- Tablas: vehiculo (1) -> orden_trabajo (N)

CREATE TABLE IF NOT EXISTS vehiculo (
    id              SERIAL PRIMARY KEY,
    placa           VARCHAR(10)  NOT NULL UNIQUE,
    marca           VARCHAR(50)  NOT NULL,
    modelo          VARCHAR(50)  NOT NULL,
    anio            INTEGER      NOT NULL CHECK (anio BETWEEN 1990 AND 2030),
    color           VARCHAR(30),
    cliente_nombre  VARCHAR(100) NOT NULL,
    fecha_registro  DATE         NOT NULL DEFAULT CURRENT_DATE
);

CREATE TABLE IF NOT EXISTS orden_trabajo (
    id              SERIAL PRIMARY KEY,
    vehiculo_id     INTEGER      NOT NULL REFERENCES vehiculo(id) ON DELETE RESTRICT,
    descripcion     VARCHAR(200) NOT NULL,
    estado          VARCHAR(20)  NOT NULL DEFAULT 'Pendiente'
                    CHECK (estado IN ('Pendiente', 'EnProceso', 'Completada', 'Cancelada')),
    costo_estimado  NUMERIC(10,2) NOT NULL DEFAULT 0 CHECK (costo_estimado >= 0),
    fecha_ingreso   DATE         NOT NULL DEFAULT CURRENT_DATE,
    fecha_entrega   DATE,
    tecnico         VARCHAR(100)
);

CREATE INDEX IF NOT EXISTS ix_orden_trabajo_vehiculo_id ON orden_trabajo(vehiculo_id);

INSERT INTO vehiculo (placa, marca, modelo, anio, color, cliente_nombre, fecha_registro) VALUES
    ('P123ABC', 'Toyota',   'Corolla',   2021, 'Blanco',   'Carlos Mendoza',  '2025-01-15'),
    ('Q456DEF', 'Honda',    'Civic',     2019, 'Negro',    'Ana Torres',     '2025-02-03'),
    ('R789GHI', 'Nissan',   'Versa',     2022, 'Rojo',     'Luis Paredes',   '2025-03-10'),
    ('S012JKL', 'Chevrolet','Onix',      2020, 'Gris',     'Maria Flores',   '2025-04-22'),
    ('T345MNO', 'Kia',      'Rio',       2023, 'Azul',     'Jorge Salazar',  '2025-05-05'),
    ('V678PQR', 'Mazda',    'Mazda3',    2018, 'Plateado', 'Elena Castro',   '2025-06-18'),
    ('W901STU', 'Suzuki',   'Swift',     2024, 'Blanco',   'Pedro Nunez',    '2025-07-30'),
    ('X234VWX', 'Hyundai',  'Accent',    2021, 'Negro',    'Sofia Rivas',    '2025-08-12');

INSERT INTO orden_trabajo (vehiculo_id, descripcion, estado, costo_estimado, fecha_ingreso, fecha_entrega, tecnico) VALUES
    (1, 'Cambio de aceite y filtro',            'Completada',  35.00, '2025-09-01', '2025-09-01', 'Roberto M.'),
    (1, 'Alineacion y balanceo de llantas',     'Pendiente',   50.00, '2025-10-10', NULL,         'Carlos T.'),
    (2, 'Revision de frenos delanteros',        'EnProceso',   120.00,'2025-10-15', NULL,         'Ana L.'),
    (3, 'Diagnostico de motor (check engine)',  'Pendiente',   80.00, '2025-10-20', NULL,         'Roberto M.'),
    (4, 'Cambio de pastillas de freno',         'Completada',  95.00, '2025-08-05', '2025-08-06', 'Carlos T.'),
    (5, 'Servicio mayor 40,000 km',             'EnProceso',   450.00,'2025-10-01', NULL,         'Ana L.'),
    (6, 'Reparacion de suspension delantera',   'Pendiente',   300.00,'2025-10-25', NULL,         'Roberto M.'),
    (7, 'Cambio de bateria',                    'Completada',  110.00,'2025-09-20', '2025-09-20', 'Carlos T.'),
    (8, 'Lavado detallado interior y exterior', 'Cancelada',   40.00, '2025-10-05', '2025-10-05', 'Ana L.');
