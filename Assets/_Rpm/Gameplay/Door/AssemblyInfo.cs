using System.Runtime.CompilerServices;

// Grant the paired test assembly access to internal test-only hooks
// (e.g. DoorController.InitForTest, ImpactScheduler.FireForTest).
// Keeps the public API clean while letting EditMode tests exercise
// state initialisation without a full MonoBehaviour lifecycle.
[assembly: InternalsVisibleTo("Rpm.Gameplay.Door.Tests")]
