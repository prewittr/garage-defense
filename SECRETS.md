# Secrets Registry

All values live in GitHub Actions secrets (or a platform vault). This file documents **names only**. Never commit values.

## Unity
- `UNITY_LICENSE` — serialized Unity Pro/Plus license
- `UNITY_EMAIL`
- `UNITY_PASSWORD`

## Backend (PlayFab)
- `PLAYFAB_TITLE_ID` — dev title (prod added at release-train cut)
- `PLAYFAB_DEV_SECRET_KEY`
- `PLAYFAB_CLOUDSCRIPT_SIGNING_KEY`

## Platform stores (added as each SKU comes online)
- `APPLE_ASC_API_KEY_ID` / `APPLE_ASC_API_ISSUER_ID` / `APPLE_ASC_API_PRIVATE_KEY`
- `GOOGLE_PLAY_SERVICE_ACCOUNT_JSON`
- `STEAM_USERNAME` / `STEAM_CONFIG_VDF`
- `PSN_*` (pending Sony onboarding)
- `XBOX_*` (pending Xbox Partner Center onboarding)

## Rules
1. No secret value ever in-repo.
2. No secret ever logged by CI (`::add-mask::` on any dynamic leak).
3. Dev and prod creds live in separate PlayFab titles and separate GitHub environments with distinct review gates.
