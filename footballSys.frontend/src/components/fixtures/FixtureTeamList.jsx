import "./FixtureTeamList.css";

const FixtureTeamList = ({ fixtures, onClose }) => {
  return (
    <div className="modal-overlay">
      <div className="modal">
        <div className="modal-header">
          <h2>Generated Fixture</h2>
          <button className="close-button" onClick={onClose}>
            &times;
          </button>
        </div>

        <div className="team-list">
          {fixtures.map((weekMatches, weekIndex) => (
            <div key={weekIndex} className="week-block">
              <h3>Week {weekIndex + 1}</h3>

              {weekMatches.map((match, matchIndex) => (
                <div key={matchIndex} className="match-item">
                  {match.homeTeamName} - {match.awayTeamName}
                </div>
              ))}
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default FixtureTeamList;