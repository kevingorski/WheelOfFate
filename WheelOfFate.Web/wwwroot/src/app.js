import React from 'react';
import SupportScheduleSpecification from './supportScheduleSpecification';
import SupportSchedule from './supportSchedule';
import { getRecentSchedules } from './supportScheduleApi';

class App extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      dailySchedules: null
    };

    this.refreshDailySchedule = this.refreshDailySchedule.bind(this);
  }

  componentWillMount() {
    this.refreshDailySchedule();
  }

  refreshDailySchedule() {
    getRecentSchedules().then(dailySchedules => this.setState({ dailySchedules }));
  }

  render() {
    return (
      <div>
        <h1>Support Wheel of Fate</h1>
        <SupportScheduleSpecification refreshDailySchedule={this.refreshDailySchedule} />
        <SupportSchedule dailySchedules={this.state.dailySchedules} />
      </div>
    );
  }
}

export default App;