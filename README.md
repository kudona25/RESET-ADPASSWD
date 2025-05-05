# Active Directory Password Enforcement Tool

This project provides a web-based tool for managing and validating password changes in an Active Directory (AD) environment. It enhances password security by enforcing strong password policies, detecting weak or compromised passwords, and integrating with external threat intelligence.

## 🔐 Purpose

The tool was developed in response to findings from HQ via CrowdStrike, which detected a significant number of compromised passwords within our AD environment. To mitigate this risk and comply with organizational security standards, I implemented a custom password validation system to guide users toward setting stronger, more secure passwords.

## ✅ Key Features

- Enforces strong password rules:
  - Minimum 10 characters
  - Must include uppercase, lowercase, digit, and special character
  - Prevents repeated characters (e.g., "aa", "11")
  - Disallows inclusion of the username in the password
  - Rejects weak/common passwords (e.g., "password123", "admin123")
  - Detects simple patterns (e.g., "1234", "abcd")

- Integration with [Have I Been Pwned API](https://haveibeenpwned.com/API/v3#PwnedPasswords) to check for known compromised passwords

- Password generation feature with random, strong password suggestion

- PowerShell scripts included:
  - Export list of users and their last password set date
  - Set `ChangePasswordAtLogon` for users who haven’t changed password in 60+ days

## 🖥️ Installation & Requirements

- IIS server (Windows Server with .NET Framework)
- RSAT: Active Directory module must be installed on the IIS server
  - Install via:  
    `Add-WindowsFeature RSAT-AD-PowerShell`
- Ensure the Application Pool Identity has permission to read AD user data
- If your network uses a proxy:
  - Configure proxy settings in the machine or web.config to allow API calls (for Pwned Passwords)
  - Example PowerShell proxy config (if needed):
    ```powershell
    [System.Net.WebRequest]::DefaultWebProxy = New-Object System.Net.WebProxy('http://yourproxy:port')
    ```

## 🚀 Usage

1. Deploy the web interface on your internal IIS server
2. Configure AD module and proxy as needed
3. Run PowerShell scripts with appropriate privileges
4. Monitor and improve rules based on feedback

