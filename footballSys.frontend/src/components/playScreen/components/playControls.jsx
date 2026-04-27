const PlayControls = ({
  onPrev,
  onNext,
  onPlayWeek,
  onPlayAll,
  isFirstWeek,
  isLastWeek,
  isWeekPlayed,
}) => {
  return (
    <div style={{ marginTop: "10px" }}>
      <button onClick={onPrev} disabled={isFirstWeek}>
        Previous
      </button>

      <button onClick={onNext} disabled={!isWeekPlayed || isLastWeek}>
        Next
      </button>

      <button onClick={onPlayWeek} disabled={isWeekPlayed}>
        Play Week
      </button>

      {/* <button onClick={onPlayAll}>
        Play All Fixture
      </button> */}
    </div>
  );
};

export default PlayControls;