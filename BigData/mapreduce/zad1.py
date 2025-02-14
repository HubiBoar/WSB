from mrjob.job import MRJob
from mrjob.step import MRStep

class MovieRatingsAverage(MRJob):
    def steps(self):
        return [
            MRStep(mapper=self.mapper_ratings,
                   reducer=self.reducer_average_rating),
        ]

    def mapper_ratings(self, _, line):
        # Read ratings.csv
        # Format: userId,movieId,rating,timestamp
        parts = line.split(',')
        if parts[0] != "userId":  # Skip header
            yield int(parts[1]), float(parts[2])  # (movieId, rating)

    def reducer_average_rating(self, movieId, ratings):
        ratings_list = list(ratings)
        avg_rating = sum(ratings_list) / len(ratings_list)
        yield movieId, avg_rating

if __name__ == '__main__':
    MovieRatingsAverage.run()
