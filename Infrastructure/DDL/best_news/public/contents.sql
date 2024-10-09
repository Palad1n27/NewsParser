create table contents
(
    id       varchar(32) not null
        primary key,
    topic_id varchar(32) not null
        references topics,
    content  text        not null
);

alter table contents
    owner to postgres;

