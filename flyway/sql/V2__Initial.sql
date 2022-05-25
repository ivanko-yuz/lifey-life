create table public.random_dare
(
    uuid              uuid         not null primary key default (uuid_generate_v4()),
    language          varchar(2)   not null,
    context           varchar(255) not null,
    experience_gained integer      not null,
    given_time        bigint
);

create table public.user
(
    uuid               uuid        not null primary key default (uuid_generate_v4()),
    default_language   varchar(2)  not null,
    first_name         varchar(50) not null,
    last_name          varchar(50) not null,
    user_name          varchar(50) not null,
    current_level      integer     not null,
    current_experience integer     not null
);

create table public.random_dares_history
(
    user_uuid                      uuid   not null,
    random_dares_uuid              uuid   not null,
    received_at_unix_utc_timestamp bigint NOT NULL,
    completed                      bool   NOT NULL,
    primary key(user_uuid, random_dares_uuid)
);

