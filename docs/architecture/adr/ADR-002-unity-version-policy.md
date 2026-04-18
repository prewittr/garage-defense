# ADR-002: Unity version policy — track latest 6000.0.x LTS patch

**Status:** Proposed
**Date:** 2026-04-17
**Deciders:** Kendra Brooks, Terrell Vaughn, Marissa Holloway
**Consulted:** Owner (Roderick Prewitt)
**Informed:** whole team

## Context
Unity Hub flagged the previously pinned `6000.0.32f1` editor with an advisory: *"Applications built with this Editor version have a known security issue. Update Editor before publishing."* No guidance was given on the exact threshold; the advisory is attached to the editor version, not a specific subsystem.

The project is Sprint 1 of a prototype. We are free to track the LTS line without migration debt because nothing of substance has been authored yet. This is the cheapest moment to set a durable policy.

## Decision Drivers
- Zero tolerance for shipping apps on a security-flagged editor.
- Minimize disruption to sprint velocity (prototype gate 2026-05-18).
- `game-ci/unity-test-runner@v4` CI support must remain stable.
- Package-manifest compatibility must remain stable; we should not introduce major-version package migrations mid-sprint.

## Options Considered

### Option A — Pin `6000.0.x` LTS; track latest patch; bump within 7 days of any security advisory
- **Pros:** Same LTS track across the whole prototype; package manifest stable (URP 17.0.3, Addressables 2.2.2, etc.); game-ci 6.0 LTS support is ubiquitous; one-time download cost.
- **Cons:** Periodic minor-version bumps required (approximately quarterly).
- **Cost:** low.

### Option B — Jump to Unity 6.4 (6000.4.3f1, Unity's "Recommended")
- **Pros:** Always on the Hub-Recommended version; already installed on Owner's machine today (zero download).
- **Cons:** Package manifest requires version audit (URP 18.x+, Addressables 2.3+, VFX Graph 18.x+); `game-ci` support for 6.4 may lag; breaks LTS stability promise mid-prototype.
- **Cost:** medium now, medium recurring (every major bump repeats).

### Option C — Pin `6000.0.32f1` indefinitely
- **Pros:** No migration work.
- **Cons:** Security advisory unresolved; Apple / Google / Sony / Microsoft store submission will reject published builds. Fails prototype → release path.
- **Cost:** Catastrophic at submission time.

## Decision
**Option A.** Pin the `6000.0.x` LTS track. Bump to the **latest patch** (`6000.0.73f1` as of this ADR) immediately. Commit to bumping within **7 calendar days** of any Unity security advisory on the pinned version for the lifetime of the prototype and Sprint 5+ polish phase.

We reconsider the track only at a pre-release stability review (Sprint 5 or later), at which point 6.1 / 6.2 / 6.3 LTS become candidate targets — each evaluated via its own ADR with package-manifest impact analysis.

## Consequences

### Positive
- Security advisory resolved. No submission risk.
- Package manifest remains stable; no mid-sprint migration work.
- CI continues to run without game-ci action pinning adjustments.
- Owner and team stay aligned on a single, predictable version cadence.

### Negative
- One-time ~5 GB download of `6000.0.73f1` on each developer's machine (Unity itself, not per-project).
- Future patch bumps require a chore commit + CI re-validation cycle (~7 min wall-clock each).

### Neutral
- No architectural change. No code migration. No test-plan rewrite.

## Compliance / Follow-up
- **Terrell Vaughn** — monitors Unity Hub for new advisories on the pinned version; files a bump commit within 7 days of any advisory and notifies Marissa.
- **Kendra Brooks** — signs off each patch bump at a Tech Design Review if the bump spans > 2 minor patches OR touches package-manifest compatibility.
- **Marissa Holloway** — tracks open version bumps on the sprint board.
- **CI contract** — every patch bump gets one full CI validation cycle on a dedicated `chore/unity-vN.N.NfN-bump` branch before merging to `develop`.

## Notes
- `ProjectSettings/ProjectVersion.txt` is the single source of truth; `game-ci/unity-test-runner@v4` auto-reads it and pulls the matching Docker image.
- The `m_EditorVersionWithRevision` line in that file is auto-rewritten by Unity on first open; we leave the parens empty on bump commits to avoid guessing hashes.

---
**Once this ADR is ACCEPTED, it is immutable.** Future changes require a new ADR that supersedes this one.
