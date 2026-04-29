import { useNavigate } from "react-router-dom";

const ContinueGameButton = () => {
  const navigate = useNavigate();

  return (
    <button onClick={() => navigate("/continue-game")}>
      Continue Game
    </button>
  );
};

export default ContinueGameButton;