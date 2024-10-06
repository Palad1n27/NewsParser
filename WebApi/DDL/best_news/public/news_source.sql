create table news_source
(
    id        varchar(32) not null
        primary key,
    news_link varchar(256)
);

alter table news_source
    owner to postgres;

