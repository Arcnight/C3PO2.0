import { connect } from 'react-redux';
import React, { Component } from 'react';
import { bindActionCreators } from 'redux';

import * as actionCreators from '../../actions/auth';

let username = '';
let password = '';
    
class Login extends Component {
    constructor(props)
    {
        super(props);

        const redirectRoute = this.props.location.query.redirect || '/login';

        this.state = { redirectUrl: redirectRoute };
    }

    OnLogin(e)
    {
        e.preventDefault();

        this.props.actions.LoginUser(username, password, this.state.redirectUrl);
    }

    OnUsernameChange(e)
    {
        username = e.target.value;
    }

    OnPasswordChange(e)
    {
        password = e.target.value;
    }

    render() {
        return (
            <div>
                <div>
                    <a>Login</a>
                </div>
                <div>
                    <form onSubmit={ this.OnLogin.bind(this) }>
                        <input type="text" name="username" placeholder="Username" onChange={ this.OnUsernameChange.bind(this) } />
                        <input type="text" name="password" placeholder="Password" onChange={ this.OnPasswordChange.bind(this) } />
                        <input type="submit" value="login" />
                    </form>
                </div>
            </div>
        );
    }
}

const mapDispatchToProps = (dispatch) => ({
  actions : bindActionCreators(actionCreators, dispatch)
});

//export default Login;
export default connect(null, mapDispatchToProps)(Login);