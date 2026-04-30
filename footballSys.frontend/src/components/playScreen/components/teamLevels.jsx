const TeamLevels = ({ teamLevels }) => {
  const sortedTeams = [...teamLevels].sort((a, b) => b.level - a.level);

  return (
    <div style={{ marginTop: "20px" }}>
      <h3>Team Levels</h3>

      {sortedTeams.map((team, index) => (
        <div key={team.id}>
          {index + 1}. {team.teamName} - Level: {team.level}
        </div>
      ))}
    </div>
  );
};

export default TeamLevels;