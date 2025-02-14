SELECT m.title, AVG(r.rating) AS avg_rating
FROM ratings r
JOIN movie m ON r.movieId = m.movieId
GROUP BY m.movieId, m.title;