use dogsitting;


CREATE TABLE Users (
    id varchar(255) PRIMARY KEY,
    firstName varchar(255),
    lastName varchar(255),
    address varchar(255),
    city varchar(255),
    phone varchar(255)
);

CREATE TABLE UserRoles (
    id varchar(255) PRIMARY KEY,
    user_id varchar(255),
    role_id varchar(255)
);

CREATE TABLE TeamUsers (
    id varchar(255) PRIMARY KEY,
    user_id varchar(255),
    team_id varchar(255)
);


CREATE TABLE Roles (
    id varchar(255) PRIMARY KEY,
    name varchar(255),
    displayName varchar(255),
    description varchar(255)
);
insert into Roles (id, name, displayName, description) values (UUID(), "Admin", "Administrateur", "A les permissions pour géré une équipe");
insert into Roles (id, name, displayName, description) values (UUID(), "SuperAdmin", "Super Administrateur", "A toutes les permissions pour géré l'ensemble des équipes.");

insert into TeamUsers (id, team_id, user_id) values (UUID(), "a7efffa3-f5c0-11ee-a26a-00155dd4f30d", "8825c601-f5c0-11ee-a26a-00155dd4f30d");
select * from TeamUsers;
select * from users;


select * from teams;
delete from teams where user_id is null;

ALTER TABLE users ADD CONSTRAINT fk_grade_id FOREIGN KEY (grade_id) REFERENCES grades(id);

CREATE TABLE Teams (
    id varchar(255) PRIMARY KEY,
    name varchar(255)
);

CREATE TABLE Teams (
    id varchar(255) PRIMARY KEY,
    name varchar(255)
);

SELECT * FROM dogsitting.clients;


insert into Users (id, firstname, lastname, address, city, phone) values ("8825c601-f5c0-11ee-a26a-00155dd4f30d", "firstname", "lastname", "address", "quebec", "555-555-5555");
insert into Users (id, firstname, lastname, address, city, phone) values ("e0b2801d-f67c-11ee-a26a-00155dd4f30d", "alexis", "guay", "address", "quebec", "555-555-5555");
insert into Teams(id, name, user_id) values (UUID(), "Annie & Annick");
insert into Teams(id, name, user_id) values (UUID(), "Alexis");
insert into Teams(id, name, user_id) values (UUID(), "Annie L.");
insert into teamusers (id, user_id, team_id) values (UUID(), "e0b2801d-f67c-11ee-a26a-00155dd4f30d", "3de9a6fc-f66f-11ee-a26a-00155dd4f30d");

select * from users;
select * from teams;
select * from teamusers;


