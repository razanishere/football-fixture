import { useLocation, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";

const PlayPage = () => {
  const location = useLocation();
  const navigate = useNavigate();

  const [currentWeek, setCurrentWeek] = useState(0);
  const [resultsByWeek, setResultsByWeek] = useState({});
  const [teamLevels, setTeamLevels] = useState([]);

  //for league table
  const [showTableModal, setShowTableModal] = useState(false);
  const [leagueTable, setLeagueTable] = useState([]);
  const [tableUnlocked, setTableUnlocked] = useState(false);

  // this is where fixtures will come from
  const fixtureData = location.state?.fixtures;
  const fixtures = fixtureData?.fixtures;
  const fixtureId = fixtureData?.fixtureId;

  // next previous button boundries
  const isFirstWeek = currentWeek === 0;
  const isLastWeek = currentWeek === fixtures.length - 1;

  // safety check
  if (!fixtures) {
    return <div>No fixtures found</div>;
  }

  const goToMainMenu = () => {
    navigate("/");
  };

  const fetchWeek = async (weekNumber) => {
    try {
      const res = await fetch(
        `http://localhost:5201/api/fixtures/${fixtureId}/weeks/${weekNumber}`,
      );

      const data = await res.json();

      setResultsByWeek((prev) => ({
        ...prev,
        [weekNumber - 1]: data,
      }));
    } catch (err) {
      console.error(err);
    }
  };

  useEffect(() => {
    fetchWeek(currentWeek + 1);
  }, [currentWeek]);

  const playWeek = async () => {
    try {
      console.group("PLAY WEEK");
      console.log("Week:", currentWeek + 1);
      console.log("Fixtures:", fixtures);

      const response = await fetch(
        `http://localhost:5201/api/simulation/play-week/${fixtureId}/${currentWeek + 1}`,
        {
          method: "POST",
        },
      );

      const data = await response.json();

      console.log("Response:", data);
      console.groupEnd();

      setResultsByWeek((prev) => ({
        ...prev,
        [currentWeek]: data.matches,
      }));

      setTeamLevels(data.teamLevels);
    } catch (error) {
      console.error(error);
    }
  };

  const playAllFixture = async () => {
    try {
      console.log("=== PLAY ALL CLICKED ===");
      console.log("Fixtures:", fixtures);

      const response = await fetch(
        `http://localhost:5201/api/simulation/play-all/${fixtureId}`,
        {
          method: "POST",
        },
      );

      const data = await response.json();

      setTeamLevels(data.teamLevels);

      
      setCurrentWeek(0);
      setResultsByWeek({});
      await fetchWeek(1);

      
      await fetchLeagueTable();
      setShowTableModal(true);
      setTableUnlocked(true);
    } catch (err) {
      console.error(err);
    }
  };
  const fetchLeagueTable = async () => {
    try {
      const res = await fetch(
        `http://localhost:5201/api/simulation/${fixtureId}/table?week=${currentWeek + 1}`,
      );

      const data = await res.json();

      console.group("LEAGUE TABLE");
      console.table(data);
      console.groupEnd();

      setLeagueTable(data);
    } catch (err) {
      console.error(err);
    }
  };

  const handlePlayWeek = async () => {
    await playWeek();
    await fetchWeek(currentWeek + 1);

    // for leagyue table
    if (currentWeek === fixtures.length - 1) {
      await fetchLeagueTable();
      setShowTableModal(true);
      setTableUnlocked(true);
    }
  };

  const handleNext = () => {
    setCurrentWeek((prev) => Math.min(prev + 1, fixtures.length - 1));
  };

  const handlePrev = () => {
    setCurrentWeek((prev) => Math.max(prev - 1, 0));
  };

  const isWeekPlayed = resultsByWeek[currentWeek]?.every(
    (m) => m.isPlayed === 1,
  );

  return (
    <div>
      <h1>Play Screen</h1>
      <button onClick={goToMainMenu}>Return to Main Menu</button>
      <h2>Week {currentWeek + 1}</h2>
      <h3>Schedule</h3>
      {fixtures[currentWeek] &&
        fixtures[currentWeek].map((match, index) => (
          <div key={index}>
            {match.homeTeamName} vs {match.awayTeamName}
          </div>
        ))}
      <button onClick={handlePrev} disabled={isFirstWeek}>
        Previous
      </button>
      <button onClick={handleNext} disabled={!isWeekPlayed || isLastWeek}>
        Next
      </button>
      <button onClick={handlePlayWeek} disabled={isWeekPlayed}>
        Play Week
      </button>

      {/* this is the section of results, i need to show it to the side of the screen.*/}
      {resultsByWeek[currentWeek] && (
        <div>
          <h3>Results</h3>
          {resultsByWeek[currentWeek].map((match, index) => (
            <div key={index}>
              {match.homeTeamName} {match.homeScore} - {match.awayScore}{" "}
              {match.awayTeamName}
            </div>
          ))}
        </div>
      )}

      <button onClick={playAllFixture}>Play All Fixture</button>

      <div style={{ marginTop: "20px" }}>
        <h3>Team Levels</h3>
        {teamLevels.map((team) => (
          <div key={team.id}>
            {team.teamName} Level: {team.level}
          </div>
        ))}
      </div>

      {/*league table section*/}
      {tableUnlocked && !showTableModal && (
        <button
          onClick={async () => {
            await fetchLeagueTable();
            setShowTableModal(true);
          }}
        >
          Show League Table
        </button>
      )}

      {showTableModal && (
        <div
          style={{
            position: "fixed",
            top: 0,
            left: 0,
            width: "100%",
            height: "100%",
            backgroundColor: "rgba(0,0,0,0.7)",
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
          }}
        >
          <div
            style={{
              background: "white",
              padding: "20px",
              borderRadius: "10px",
              width: "500px",
            }}
          >
            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
              }}
            >
              <h2>League Table</h2>

              <button
                onClick={() => setShowTableModal(false)}
                style={{
                  background: "transparent",
                  border: "none",
                  fontSize: "20px",
                  cursor: "pointer",
                }}
              >
                ✖
              </button>
            </div>

            <table style={{ width: "100%", borderCollapse: "collapse" }}>
              <thead>
                <tr style={{ borderBottom: "2px solid black" }}>
                  <th>#</th>
                  <th>Team</th>
                  <th>P</th>
                  <th>W</th>
                  <th>D</th>
                  <th>L</th>
                  <th>GS</th>
                  <th>GC</th>
                  <th>Pts</th>
                </tr>
              </thead>

              <tbody>
                {[...leagueTable]
                  .sort((a, b) => {
                    if (b.points !== a.points) return b.points - a.points;
                    if (b.goalDifference !== a.goalDifference)
                      return b.goalDifference - a.goalDifference;
                    return b.goalsScored - a.goalsScored;
                  })
                  .map((team, index) => (
                    <tr
                      key={team.teamId}
                      style={{
                        textAlign: "center",
                        backgroundColor:
                          index === 0 ? "#ffec3d" : "transparent",
                        fontWeight: index === 0 ? "bold" : "normal",
                      }}
                    >
                      <td>{index + 1}</td>
                      <td style={{ textAlign: "left" }}>{team.teamName}</td>
                      <td>{team.played}</td>
                      <td>{team.wins}</td>
                      <td>{team.draws}</td>
                      <td>{team.losses}</td>
                      <td>{team.goalsScored}</td>
                      <td>{team.goalsConceded}</td>
                      <td>
                        <strong>{team.points}</strong>
                      </td>
                    </tr>
                  ))}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
};

export default PlayPage;
