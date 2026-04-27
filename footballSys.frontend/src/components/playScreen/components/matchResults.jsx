const MatchResults = ({ results }) => {
  if (!results) return null;

  return (
    <div>
      <h3>Results</h3>
      {results.map((match, index) => (
        <div key={index}>
          {match.homeTeamName} {match.homeScore} - {match.awayScore}{" "}
          {match.awayTeamName}
        </div>
      ))}
    </div>
  );
};

export default MatchResults;