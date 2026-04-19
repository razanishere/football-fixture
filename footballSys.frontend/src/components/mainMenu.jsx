import ListTeamsButton from "./ListTeamsButton";
import GenerateFixtureButton from "./fixtures/GenerateFixtureButton";

function MainMenu() {
  return (
    <div>
      <h1>Main Menu</h1>

      <GenerateFixtureButton />

      <ListTeamsButton />
      
    </div>
  );
}

export default MainMenu;