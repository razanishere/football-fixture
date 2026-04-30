import { Routes, Route } from "react-router-dom";
import MainMenu from "./components/mainMenu";
import PlayPage from "./components/playScreen/PlayPage";
import ContinueGamePage from "./components/continue/ContinueGamePage";

//* we do the routing here

function App() {
  return (
    <Routes>
      <Route path="/" element={<MainMenu />} />
      <Route path="/play/:fixtureId" element={<PlayPage />} />
      <Route path="/continue-game" element={<ContinueGamePage />} />
    </Routes>
  );
}

export default App;