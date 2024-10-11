create table users
(
    id                            uuid        not null
        primary key,
    login                         varchar(32) not null
        unique,
    password                      varchar(32) not null,
    role                          integer     not null,
    refresh_token                 varchar,
    refresh_token_creation_date   timestamp,
    refresh_token_expiration_date timestamp
);

alter table users
    owner to postgres;

