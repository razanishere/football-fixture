import { useLocation, useNavigate } from "react-router-dom";
import { useState } from "react";

const PlayPage = () => {
  const location = useLocation();
  const navigate = useNavigate();

  const goToMainMenu = () => {
    navigate("/"); // adjust this if your main menu route is different
  };

  // this is where fixtures will come from
  const data = location.state?.fixtures;
  const fixtures = data?.fixtures;
  console.log("PlayPage data:", fixtures);
  const fixtureId = data?.fixtureId;
  const [currentWeek, setCurrentWeek] = useState(0);
  const [resultsByWeek, setResultsByWeek] = useState({});

  // safety check
  if (!fixtures) {
    return <div>No fixtures found</div>;
  }

  const playWeek = async () => {
    try {
      const response = await fetch(
        `http://localhost:5201/api/simulation/play-week/${fixtureId}/${currentWeek + 1}`,
        {
          method: "POST",
        },
      );

      const data = await response.json();

      console.log("PLAY RESULT:", data);

      setResultsByWeek((prev) => ({
        ...prev,
        [currentWeek]: data.matches,
      }));
    } catch (error) {
      console.error(error);
    }
  };

  const handleNext = () => {
    setCurrentWeek((prev) => Math.min(prev + 1, fixtures.length - 1));
  };

  const handlePrev = () => {
    setCurrentWeek((prev) => Math.max(prev - 1, 0));
  };

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

      <button onClick={handlePrev}>Previous</button>
      <button onClick={handleNext}>Next</button>
      <button onClick={playWeek} disabled={resultsByWeek[currentWeek]}>
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
    </div>
  );
};

export default PlayPage;
