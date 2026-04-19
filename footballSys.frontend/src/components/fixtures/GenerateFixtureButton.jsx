import { useState } from "react";
import FixtureTeamSelectionModal from "./FixtureTeamSelectionModal";
import FixtureTeamList from "./FixtureTeamList";
import { useNavigate } from "react-router-dom";

const GenerateFixtureButton = () => {
  const [showModal, setShowModal] = useState(false);

  //* store data coming from the FixtureTeamSelectionModal
  const [fixtures, setFixtures] = useState(null);

  //* is the selection menu open?
  const [isSelectionOn, setIsSelectionOn] = useState(false);

  const navigate = useNavigate();

  return (
    <>
      <button onClick={() => setIsSelectionOn(true)}>New Game</button>
      {isSelectionOn && (
        <FixtureTeamSelectionModal
          onClose={() => setIsSelectionOn(false)}
          onGenerate={(data) => {
            setFixtures(data);
            setIsSelectionOn(false);
            ////setShowModal(true);

            navigate("/play", { state: { fixtures: data } });
          }}
        />
      )}

      {/* render FixtureTeamList and give it fixtures*/}
      {showModal && fixtures && (
        <FixtureTeamList
          fixtures={fixtures} // pass the generated fixtures
          onClose={() => setShowModal(false)} // close button works now
        />
      )}
    </>
  );
};

export default GenerateFixtureButton;
