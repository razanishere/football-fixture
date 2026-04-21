import { useLocation, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";

const PlayPage = () => {
  const location = useLocation();
  const navigate = useNavigate();

  const [currentWeek, setCurrentWeek] = useState(0);
  const [resultsByWeek, setResultsByWeek] = useState({});

  // this is where fixtures will come from
  const data = location.state?.fixtures;
  const fixtures = data?.fixtures;
  console.log("PlayPage data:", fixtures);
  const fixtureId = data?.fixtureId;

  //next previous button boundries
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

  const playAllFixture = async () => {
    try {
      const response = await fetch(
        `http://localhost:5201/api/simulation/play-all/${fixtureId}`,
        {
          method: "POST",
        },
      );

      const data = await response.json();

      console.log("PLAY ALL RESULT:", data);

      await fetchWeek(currentWeek + 1);
    } catch (err) {
      console.error(err);
    }

    setCurrentWeek(0);
    setResultsByWeek({});
    await fetchWeek(1);
  };

  const handlePlayWeek = async () => {
    await playWeek();
    await fetchWeek(currentWeek + 1);
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
    </div>
  );
};

export default PlayPage;
