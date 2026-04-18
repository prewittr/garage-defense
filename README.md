# Rpm: Redline / Deadline

A claustrophobic survival-thriller prototype. One greasy mechanic. One garage door. One last shot at escape.

## Current Sprint
**Sprint 0 — Tactile Terror.** Prove the Door tension-relief loop. Backlog tickets RPM-001 through RPM-008.

## Team & Protocol
- Specialist agents: [.claude/agents/](.claude/agents/)
- Operating protocol: [CLAUDE.md](CLAUDE.md)

## Branching (GitFlow)
- `main` — release tags only, signed, QA-signed.
- `develop` — integration branch.
- `feature/*` — feature work. Current: `feature/tactile-terror`.
- `release/*` / `hotfix/*` — stabilization.

## Quick Start
1. Install Unity Hub and Unity 6000.0.73f1 (or the version pinned in `ProjectSettings/ProjectVersion.txt`).
2. Clone with LFS: `git lfs install && git clone <repo>`.
3. Open the project root in Unity Hub.
