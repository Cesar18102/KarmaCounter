CREATE TABLE users(
  id INT PRIMARY KEY IDENTITY(1,1),
  login NVARCHAR(32) NOT NULL UNIQUE,
  pwd NVARCHAR(64) NOT NULL,
  email NVARCHAR(128) NOT NULL,
  global_karma FLOAT NOT NULL DEFAULT 0
);

CREATE TABLE group_owners(
    id INTEGER PRIMARY KEY IDENTITY(1,1),
    user_id INTEGER NOT NULL,
    public_key NVARCHAR(64) NOT NULL,
    private_key NVARCHAR(64) NOT NULL,
    FOREIGN KEY(user_id) REFERENCES users(id) ON UPDATE CASCADE ON DELETE CASCADE,
);

CREATE TABLE karma_groups(
    id INTEGER PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NOT NULL,
    description NTEXT,
    owner_id INTEGER NOT NULL,
    is_public BIT DEFAULT 1,
	is_local BIT DEFAULT 1,
    FOREIGN KEY(owner_id) REFERENCES group_owners(id) ON UPDATE CASCADE
);

CREATE TABLE group_members(
    id INTEGER PRIMARY KEY IDENTITY(1,1),
    group_id INTEGER NOT NULL,
    user_id INTEGER NOT NULL,
    local_karma FLOAT NOT NULL DEFAULT 0,
    custom_data NTEXT,
    FOREIGN KEY(group_id) REFERENCES karma_groups(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(user_id) REFERENCES users(id) ON UPDATE NO ACTION ON DELETE NO ACTION
);

CREATE TABLE group_rules_fees(
    id INTEGER PRIMARY KEY IDENTITY(1,1),
    rule_title NVARCHAR(255) NOT NULL,
    rule_text NTEXT,
    fee_formula NTEXT NOT NULL,
    group_id INTEGER NOT NULL,
    FOREIGN KEY(group_id) REFERENCES karma_groups(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE rules_actions(
    id INTEGER PRIMARY KEY IDENTITY(1,1),
    user_id INTEGER NOT NULL,
    rule_id INTEGER NOT NULL,
    date_time DATETIME NOT NULL,
    violated BIT DEFAULT 0,
	fee FLOAT NOT NULL DEFAULT 0,
    FOREIGN KEY(group_id) REFERENCES karma_groups(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(user_id) REFERENCES users(id) ON UPDATE NO ACTION ON DELETE NO ACTION,
    FOREIGN KEY(rule_id) REFERENCES group_rules_fees(id) ON UPDATE NO ACTION ON DELETE NO ACTION
);

CREATE TABLE group_invitations(
    id INTEGER PRIMARY KEY IDENTITY(1,1),
    user_id INTEGER NOT NULL,
    group_id INTEGER NOT NULL,
    FOREIGN KEY(user_id) REFERENCES users(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(group_id) REFERENCES karma_groups(id) ON UPDATE NO ACTION ON DELETE CASCADE
);

CREATE TABLE log(
    caption NVARCHAR(255) NOT NULL,
    date_time_code DATETIME,
    message NTEXT
);


-- DELETE FROM group_invitations;
-- DELETE FROM rules_actions;
-- DELETE FROM group_rules_fees;
-- DELETE FROM group_members;
-- DELETE FROM karma_groups;
-- DELETE FROM group_owners;
-- DELETE FROM users;
-- DELETE FROM log;