ratings = LOAD 'data.csv' USING PigStorage(',') AS (userId:int, movieId:int, rating:float, timestamp:long);

grouped_ratings = GROUP ratings BY movieId;

average_ratings = FOREACH grouped_ratings GENERATE group AS movieId, AVG(ratings.rating) AS avg_rating;

DUMP average_ratings;