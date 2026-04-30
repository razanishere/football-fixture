import { useParams, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import MatchSchedule from "./components/MatchSchedule";
import MatchResults from "./components/MatchResults";
import TeamLevels from "./components/TeamLevels";
import LeagueTableModal from "./components/LeagueTableModal";
import PlayControls from "./components/PlayControls";
import {
  fetchWeekApi,
  playWeekApi,
  playAllApi,
  fetchLeagueTableApi,
  finishFixtureApi,
  fetchFixtureByIdApi,
} from "./services/playApi";

const PlayPage = () => {
  ////const location = useLocation();
  const navigate = useNavigate();
  const { fixtureId } = useParams();

  const [currentWeek, setCurrentWeek] = useState(0);
  const [resultsByWeek, setResultsByWeek] = useState({});
  const [teamLevels, setTeamLevels] = useState([]);

  //for league table
  const [showTableModal, setShowTableModal] = useState(false);
  const [leagueTable, setLeagueTable] = useState([]);
  const [tableUnlocked, setTableUnlocked] = useState(false);

  const [fixtures, setFixtures] = useState([]);


  // next previous button boundries
  const isFirstWeek = currentWeek === 0;
  const isLastWeek = currentWeek === fixtures.length - 1;


  const goToMainMenu = () => {
    navigate("/");
  };

  const fetchWeek = async (weekNumber) => {
    try {
      const data = await fetchWeekApi(fixtureId, weekNumber);

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

  const finishFixture = async () => {
    try {
      await finishFixtureApi(fixtureId);
    } catch (err) {
      console.error(err);
    }
  };

  useEffect(() => {
    const loadFixture = async () => {
      try {
        const data = await fetchFixtureByIdApi(fixtureId);
        setFixtures(data.fixtures);
      } catch (err) {
        console.error(err);
      }
    };

    if (fixtureId) {
      loadFixture();
    }
  }, [fixtureId]);


  const playWeek = async () => {
    try {
      console.group("PLAY WEEK");
      console.log("Week:", currentWeek + 1);

      const data = await playWeekApi(fixtureId, currentWeek + 1);

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
      const data = await playAllApi(fixtureId);

      setTeamLevels(data.teamLevels);

      setCurrentWeek(0);
      setResultsByWeek({});
      await fetchWeek(1);

      await fetchLeagueTable();
      await finishFixture();
      setShowTableModal(true);
      setTableUnlocked(true);
    } catch (err) {
      console.error(err);
    }
  };

  const fetchLeagueTable = async () => {
    try {
      const data = await fetchLeagueTableApi(fixtureId, currentWeek + 1);

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
      await finishFixture();
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

  if (!fixtures || fixtures.length === 0) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <h1>Play Screen</h1>
      <button onClick={goToMainMenu}>Return to Main Menu</button>

      <h2>Week {currentWeek + 1}</h2>
      <MatchSchedule fixtures={fixtures} currentWeek={currentWeek} />
      <PlayControls
        onPrev={handlePrev}
        onNext={handleNext}
        onPlayWeek={handlePlayWeek}
        onPlayAll={playAllFixture}
        isFirstWeek={isFirstWeek}
        isLastWeek={isLastWeek}
        isWeekPlayed={isWeekPlayed}
      />

      {/*TODO: this is the section of results, i need to show it to the side of the screen.*/}
      <MatchResults results={resultsByWeek[currentWeek]} />

      <button onClick={playAllFixture}>Play All Fixture</button>

      <TeamLevels teamLevels={teamLevels} />

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

      <LeagueTableModal
        show={showTableModal}
        onClose={() => setShowTableModal(false)}
        leagueTable={leagueTable}
      />
    </div>
  );
};

export default PlayPage;
