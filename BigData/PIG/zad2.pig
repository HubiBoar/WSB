ratings = LOAD 'ratings.csv' USING PigStorage(',') AS (userId:int, movieId:int, rating:float, timestamp:long);

movies = LOAD 'movies.csv' USING PigStorage(',') AS (movieId:int, title:chararray, genres:chararray);

grouped_ratings = GROUP ratings BY movieId;

average_ratings = FOREACH grouped_ratings GENERATE group AS movieId, AVG(ratings.rating) AS avg_rating;

joined_data = JOIN average_ratings BY movieId, movies BY movieId;

final_result = FOREACH joined_data GENERATE movies::movieId AS movieId, movies::title AS title, average_ratings::avg_rating AS avg_rating;

DUMP final_result;