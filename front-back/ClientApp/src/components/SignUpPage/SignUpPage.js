import "./SignUpPage.css";

function SignUpPage() {
  return (
    <div className="sign-up-container">
      <h1>Real World App</h1>
      <p>Sign Up</p>
      <form className="sign-up-form">
        <input type="text" placeholder="First name*" required></input>
        <input type="text" placeholder="Last name*" required></input>
        <input type="text" placeholder="Username*" required></input>
        <input type="password" placeholder="Password*" required></input>
        <input type="password" placeholder="Confirm Password*" required></input>
        <button type="submit">SIGN UP</button>
      </form>
    </div>
  );
}

export default SignUpPage;
