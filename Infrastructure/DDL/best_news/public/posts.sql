create table posts
(
    id            varchar(128) not null
        constraint topics_pkey
            primary key,
    name          varchar(256) not null,
    creation_date timestamp    not null,
    content       varchar(10000)
);

alter table posts
    owner to postgres;

