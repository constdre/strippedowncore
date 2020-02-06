class Shareable extends React.Component {
    render() {
        return <div>Hello world!</div>;
    }
}
const container = document.getElementById('react_root');
ReactDOM.render(<Shareable />, container);