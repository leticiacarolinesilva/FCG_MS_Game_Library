-- Enable UUID extension
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Create schema
CREATE SCHEMA IF NOT EXISTS game_platform;

-- Set default permissions
ALTER DEFAULT PRIVILEGES IN SCHEMA game_platform
GRANT ALL ON TABLES TO gameuser;

ALTER DEFAULT PRIVILEGES IN SCHEMA game_platform
GRANT ALL ON SEQUENCES TO gameuser;