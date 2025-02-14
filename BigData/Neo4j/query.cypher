MATCH (p:Person)-[:BELONGS_TO]->(b:Board {name: "Prokurenci"})
RETURN p.name AS Prokurent, p.title AS Title