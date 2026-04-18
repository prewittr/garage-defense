---
id: owner-022
from: Marissa Holloway (command-relay)
to: Owner (Roderick Prewitt)
sent: 2026-04-18T00:20Z
subject: Working-copy policy locked — single copy at C:\development\garage-defense (WSL via /mnt/c)
thread: sprint-1-kickoff
in-reply-to: owner-021
status: answered
decision: (A) — single working copy at C:\development\garage-defense; WSL operates via /mnt/c mount; /home/rprewitt/development/gd retired
---

[STATUS]: Green — working-copy policy resolved before any drift accumulated. ADR-003 filed.

[THE WIN]: Clean migration with zero lost work.
- **Both clones synchronized with `origin` at decision time** — no divergent commits, no merge conflicts. Verified HEAD SHAs:
  - WSL side (`/home/rprewitt/development/gd`) on `feature/tactile-terror` @ `353c993` ↔ `origin/feature/tactile-terror` ✓
  - Windows side (`C:\development\garage-defense` / `/mnt/c/development/garage-defense`) on `feat/rpm-007` @ `7804e6c` ↔ `origin/feat/rpm-007` ✓
- **Windows clone has already generated the Unity Editor-deferred scaffolding** just from first-open: `.meta` files for all asmdef modules, `Packages/packages-lock.json` resolved, URP default assets created. The scene + Addressables work from Steps A–D of my Editor checklist still needs to happen, but everything Unity generates automatically is in place.
- **[ADR-003](../../docs/architecture/adr/ADR-003-single-working-copy.md) filed** (Proposed) capturing the policy, the UNC-path failure mode, options considered, and Sprint 2 wrap action (delete `/home/rprewitt/development/gd`).
- **All my future agent invocations and Bash calls run against `/mnt/c/development/garage-defense`** from this moment forward.

[REDLINE]: Two transitional items:
1. **Comms cards 017–021 live on `feature/tactile-terror`** but Windows clone is currently on `feat/rpm-007`. When `feat/rpm-007` eventually merges via PR #1, both branches will reconverge and the full comms thread will appear everywhere. Temporarily, viewing the full Inbox archive requires looking at `feature/tactile-terror` on GitHub.
2. **`/home/rprewitt/development/gd` is now orphaned.** Per ADR-003 it stays for one sprint as a fallback, then gets deleted on Sprint 2 wrap. No new commits land there.

[DECISION REQUIRED]: None — your (A) is already actioned and codified. ADR-003 moves from Proposed to Accepted at the Sprint 1 Tech Design Review (Mon 2026-04-20 14:00) alongside ADR-001 (VContainer) and ADR-002 (Unity version policy).

Next: you continue the Editor checklist (Steps A–E — scenes, Addressables, Build Settings, Save Project) on the Windows clone. Meta files Unity has already generated will be picked up in the same commit as your scene work.
