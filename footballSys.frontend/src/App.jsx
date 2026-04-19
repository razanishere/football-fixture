import { Routes, Route } from "react-router-dom";
import MainMenu from "./components/mainMenu";
import PlayPage from "./components/playScreen/PlayPage";

//* we do the routing here

function App() {
  return (
    <Routes>
      <Route path="/" element={<MainMenu />} />
      <Route path="/play" element={<PlayPage />} />
    </Routes>
  );
}

export default App;