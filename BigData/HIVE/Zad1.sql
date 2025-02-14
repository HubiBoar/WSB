SELECT movieId, AVG(rating) AS avg_rating
FROM ratings
GROUP BY movieId;