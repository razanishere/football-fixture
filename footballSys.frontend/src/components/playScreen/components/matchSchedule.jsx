const MatchSchedule = ({ fixtures, currentWeek }) => {
  if (!fixtures[currentWeek]) return null;

  return (
    <div>
      <h3>Schedule</h3>
      {fixtures[currentWeek].map((match, index) => (
        <div key={index}>
          {match.homeTeamName} vs {match.awayTeamName}
        </div>
      ))}
    </div>
  );
};

export default MatchSchedule;