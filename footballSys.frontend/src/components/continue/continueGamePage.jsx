import { useEffect, useState } from "react";
import { getFixtures } from "./services/fixtureInfoApi";
import { useNavigate } from "react-router-dom";

const ContinueGamePage = () => {
  const navigate = useNavigate();
  const [fixtures, setFixtures] = useState([]);

  const goToMainMenu = () => {
    navigate("/");
  };

  useEffect(() => {
    fetchFixtures();
  }, []);

  const fetchFixtures = async () => {
    try {
      const data = await getFixtures();
      setFixtures(data);
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <div>
      <h2>Continue Game</h2>

      <button onClick={goToMainMenu}>Return to Main Menu</button>

      <div className="fixture-grid">
        {fixtures.map((fixture) => (
          <div key={fixture.id} className="fixture-card">
            <h3>{fixture.name}</h3>

            <p>Status: {fixture.isFinished ? "Finished" : "In Progress"}</p>

            {!fixture.isFinished && (
              <button onClick={() => navigate(`/play/${fixture.id}`)}>
                Play
              </button>
            )}
          </div>
        ))}
      </div>
    </div>
  );
};

export default ContinueGamePage;
