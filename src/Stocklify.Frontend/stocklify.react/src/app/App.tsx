import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'
import StartPage from '../pages/StartPage/StartPage'

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<StartPage />} />
      </Routes>
    </Router>
  )
}

export default App
