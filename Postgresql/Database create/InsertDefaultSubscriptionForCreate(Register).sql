INSERT INTO "Subscription" (
    "Id", 
    "Price", 
    "Description", 
    "Name", 
    "StartDate", 
    "Duration", 
    "CreatedBy", 
    "UpdatedBy", 
    "DiscountId"
) 
VALUES (
    '00000000-0000-0000-0000-000000000000', 
    '0',                              
    'Default on create! 0%',
	'Default0',
    '2024-12-01',       -- Datum početka
    365,   
    '123e4567-e89b-12d3-a456-426614174000', -- UUID osobe koja kreira zapis
    '123e4567-e89b-12d3-a456-426614174000', -- UUID osobe koja ažurira zapis
    '22222222-2222-2222-2222-222222222222'  -- UUID vezanog popusta
);
