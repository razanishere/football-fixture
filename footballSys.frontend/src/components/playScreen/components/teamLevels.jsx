const TeamLevels = ({ teamLevels }) => {
  return (
    <div style={{ marginTop: "20px" }}>
      <h3>Team Levels</h3>
      {teamLevels.map((team) => (
        <div key={team.id}>
          {team.teamName} Level: {team.level}
        </div>
      ))}
    </div>
  );
};

export default TeamLevels;