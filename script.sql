use dogsitting;


DROP TABLE roles;
DROP TABLE userroles;
DROP TABLE teamusers;
DROP TABLE reservations;
DROP TABLE teams;
DROP TABLE USERS;

CREATE TABLE Roles (
    id varchar(255) PRIMARY KEY,
    name varchar(255),
    displayName varchar(255),
    description varchar(255)
);
insert into Roles (id, name, displayName, description) values (UUID(), "Admin", "Administrateur", "A les permissions pour géré une équipe");
insert into Roles (id, name, displayName, description) values (UUID(), "SuperAdmin", "Super Administrateur", "A toutes les permissions pour géré l'ensemble des équipes.");

CREATE TABLE Users (
    id varchar(255) PRIMARY KEY,
    firstName varchar(255),
    lastName varchar(255),
    email varchar(255),
    address varchar(255),
    city varchar(255),
    phone varchar(255)
);

CREATE TABLE UserRoles (
    id varchar(255) PRIMARY KEY,
    user_id varchar(255),
    role_id varchar(255)
);

CREATE TABLE Teams (
    id varchar(255) PRIMARY KEY,
    name varchar(255)
);

CREATE TABLE TeamUsers (
    id varchar(255) PRIMARY KEY,
    user_id varchar(255),
    team_id varchar(255),
	CONSTRAINT fk_user_id FOREIGN KEY (user_id) REFERENCES users(id),
    CONSTRAINT fk_team_id FOREIGN KEY (team_id) REFERENCES teams(id)
);


#ALTER TABLE users ADD CONSTRAINT fk_grade_id FOREIGN KEY (grade_id) REFERENCES grades(id);

CREATE TABLE Calendars (
    id varchar(255) PRIMARY KEY
);

CREATE TABLE Reservations (
    id varchar(255) PRIMARY KEY,
    dateFrom datetime,
    dateTo datetime,
    lodgerCount integer,
    user_id varchar(255),
    team_id varchar(255),
    CONSTRAINT fk_calendar_user_id FOREIGN KEY (user_id) REFERENCES users(id),
    CONSTRAINT fk_calendar_team_id FOREIGN KEY (team_id) REFERENCES teams(id)
);


insert into Users (id, firstname, lastname, address, city, phone) values ("8825c601-f5c0-11ee-a26a-00155dd4f30d", "firstname", "lastname", "address", "quebec", "555-555-5555");
insert into Users (id, firstname, lastname, address, city, phone) values ("e0b2801d-f67c-11ee-a26a-00155dd4f30d", "alexis", "guay", "address", "quebec", "555-555-5555");
insert into Users (id, firstname, lastname, address, city, phone) values ("e0b2801d-f67c-11ee-a26a-00155dd4f39d", "clientA", "clientA", "address", "quebec", "555-555-5555");
#UUID()
insert into Teams(id, name) values ("2e731e68-f682-11ee-a26a-00155dd4f30d", "Annie & Annick");
insert into Teams(id, name) values ("2efa6903-f682-11ee-a26a-00155dd4f30d", "Alexis");
insert into Teams(id, name) values ("2f68968c-f682-11ee-a26a-00155dd4f30d", "Annie L.");
insert into teamusers (id, user_id, team_id) values (UUID(), "e0b2801d-f67c-11ee-a26a-00155dd4f30d", "2e731e68-f682-11ee-a26a-00155dd4f30d");
insert into reservations (id, dateFrom, dateTo, lodgerCount, user_id, team_id) values (UUID(), "2024-01-01", "2024-12-01", 5, "e0b2801d-f67c-11ee-a26a-00155dd4f39d", "2e731e68-f682-11ee-a26a-00155dd4f30d");

select * from users;
select * from teams;
select * from teamusers;
select * from reservations;

