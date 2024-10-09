create table topics
(
    id            varchar(32)  not null
        primary key,
    source_id     varchar(32)  not null
        references news_source,
    name          varchar(256) not null,
    creation_date timestamp    not null
);

alter table topics
    owner to postgres;

