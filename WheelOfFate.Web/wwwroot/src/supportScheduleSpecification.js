import React from 'react';
import { generateSchedule } from './supportScheduleApi';
import SupportSchedule from './supportSchedule';

class SupportScheduleSpecification extends React.Component {
  constructor(props) {
    super(props);

    const today = new Date();

    this.state = {
      specification: {
        date: today.toISOString().substring(0, 10),
        requireSingleShiftPerEngineerPerDay: true,
        requireDayOffBetweenDaysWithShifts: true,
        requireTwoShiftsInTwoWeeks: true
      },
      error: null,
      spinning: false
    };

    this.handleChange = this.handleChange.bind(this);
  }

  handleChange(event) {
    const target = event.target;
    const name = target.name;
    const value = target.type == 'checkbox' ? target.checked : target.value;

    const newState = {
      ...this.state,
      specification: {
        ...this.state.specification,
        [name]: value
      }
    };

    this.setState(newState);
  }

  handleSubmitSchedule() {
    this.setState({error: null, isSpinning: true});

    generateSchedule(this.state.specification)
      .then(this.props.refreshDailySchedule)
      .catch(err => err.response.json()
          .then(body => this.setState({ error: body.message }))
          .catch(() => this.setState({ error: err})))
      .finally(() => this.setState({ isSpinning: false }));
  }

  render() {
    return (
      <section>
        <h1>Support Schedule Settings</h1>
        <div>
          <label>
            Date to schedule:&nbsp;
            <input name="date" type="date" value={this.state.specification.date} onChange={this.handleChange} />
          </label>
        </div>
        <div>
          <label>
            <input 
              name="requireSingleShiftPerEngineerPerDay"
              type="checkbox"
              checked={this.state.specification.requireSingleShiftPerEngineerPerDay}
              onChange={this.handleChange} />
            Limit engineer to single shift per day
          </label>
        </div>
        <div>
          <label>
            <input 
              name="requireDayOffBetweenDaysWithShifts"
              type="checkbox"
              checked={this.state.specification.requireDayOffBetweenDaysWithShifts}
              onChange={this.handleChange} />
            Require day off between support days
          </label>
        </div>
        <div>
          <label>
            <input 
              name="requireTwoShiftsInTwoWeeks"
              type="checkbox"
              checked={this.state.specification.requireTwoShiftsInTwoWeeks}
              onChange={this.handleChange} />
            Require two support days in two weeks
          </label>
        </div>
        <div className="buttons">
          <button onClick={() => this.handleSubmitSchedule()} disabled={this.state.isSpinning}>Spin the Wheel</button>
        </div>
        {this.state.error &&
          <div className="error-text">{this.state.error}</div>
        }
      </section>
    );
  }
}

export default SupportScheduleSpecification;