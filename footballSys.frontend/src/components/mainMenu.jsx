import ListTeamsButton from "./ListTeamsButton";
import GenerateFixtureButton from "./fixtures/GenerateFixtureButton";
import ContinueGameButton from "./continue/ContinueGameButton";

function MainMenu() {
  return (
    <div>
      <h1>Main Menu</h1>

      <GenerateFixtureButton />
      <ContinueGameButton />
      <ListTeamsButton />
    </div>
  );
}

export default MainMenu;
