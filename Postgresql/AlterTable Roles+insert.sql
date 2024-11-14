ALTER TABLE "Role"
DROP COLUMN "CreatedBy",
DROP COLUMN "UpdatedBy",
DROP COLUMN "DatedUpdated";

INSERT INTO "Role" ("Id", "RoleName", "DateCreated", "IsActive")
VALUES ('22222222-2222-2222-2222-222222222222', 'User', CURRENT_TIMESTAMP, 'True');

INSERT INTO "Role" ("Id", "RoleName", "DateCreated", "IsActive")
VALUES ('33333333-3333-3333-3333-333333333333', 'Coach', CURRENT_TIMESTAMP, 'True');